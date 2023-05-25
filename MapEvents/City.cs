using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace Mononoke
{
    class City : ExpandableMapEvent, IDraggable
    {
        static int Max = 10;
        int _Current = 0;

        static int GrowthThreshold = 5;
        static int TurnsToGrow = 1;
        int growthTickDown = 5;

        List<Vector2> FreeWorkers; // positions of free workers.

        MapHolder mMaps;
        MapUnitHolder mUnitHolder;
        MapUnitTypeHolder mUnitTypeHolder;
        int Current
        {
            set
            {
                if (value >= Max)
                    _Current = Max;
                else
                    _Current = value;
            }
            get
            {
                return _Current;
            }
        }
        public City( Vector2 pos, MapHolder maps, MapUnitHolder unitHolder, MapUnitTypeHolder unitTypeHolder, Actor owner ) : base ( pos, owner )
        {
            AllowedTerrains = new eTerrainType[] { eTerrainType.Forest, eTerrainType.Road }; // do not add other building types to this!!
            mUnitHolder = unitHolder;
            mMaps = maps;
            mUnitTypeHolder = unitTypeHolder;
            FreeWorkers = new List<Vector2>();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw( spriteBatch );
            spriteBatch.DrawString(Mononoke.Font, Current.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black);
        }
        public override void OnClick(Player clicker)
        {
            if ( Current < Max )
            {
                clicker.Food--;
                Current++;
            }
        }
        protected override void OnExpire()
        {
            if ( Current - 1 < 0 )
            {
                if ( mOwner != null )
                    mOwner.Stability--;
            }
            else
            { 
                Current--;
            }
            if ( Current >= GrowthThreshold )
            {
                growthTickDown--;
                if ( growthTickDown <= 0 )
                {
                    TryExpand( mMaps );
                    growthTickDown = TurnsToGrow;
                }
            }
            else
            {
                growthTickDown = TurnsToGrow;
            }
        }
        public override bool TryExpand(MapHolder mh)
        {
            Vector2 pos;
            if ( base.TryExpand( mh, out pos ) )
            { 
                FreeWorkers.Add( pos );
                return true;
            }
            return false;
        }
        public override bool TryLink(MapEvent partner, MapHolder maps, List<Vector2> path )
        {
            ExpandableMapEvent e = (ExpandableMapEvent)partner;
            if ( FreeWorkers.Count == 0 )
                return false;
            if ( e.TryExpand( maps ) )
            { 
                ConsumeWorker();
                List<Vector2> result = new List<Vector2>();
                foreach (Vector2 p in path)
                {
                    if ( TerrainTypeMap.Buildable(maps.GetTerrainAt(p)) )
                    {
                        result.Add( p );
                    }
                }
                maps.SetTerrainAt(result, eTerrainType.Road);
                return true;
            }
            return false;
        }
        protected override eTerrainType GetExpansionType()
        {
            return eTerrainType.Urban;
        }
        public override bool TryGetRadialMenu(out RadialMenu r)
        {
            r = null;
            if ( mOwner == null )
                return false;
            if (FreeWorkers.Count > 0)
            { 
                List<RadialMenuItem> l = new List<RadialMenuItem>();
                l.Add( new RadialMenuItem(
                      MapHolder.PIXELS_PER_TILE * Origin - new Vector2(0, MapHolder.PIXELS_PER_TILE)
                    , TextureAssetManager.GetUnitSpriteByName("soldier")
                    , () => { 
                        this.AddSoldier();
                       }
                    )
                );
                l.Add(new RadialMenuItem(
                      MapHolder.PIXELS_PER_TILE * Origin - new Vector2(MapHolder.PIXELS_PER_TILE, 0)
                    , TextureAssetManager.GetIconByName("enamel")
                    , () => {
                        this.AddFoundry();
                        }
                    )
                );
                //l.Add(new RadialMenuItem(
                //      MapHolder.PIXELS_PER_TILE * Origin - new Vector2(MapHolder.PIXELS_PER_TILE, 0)
                //    , TextureAssetManager.GetUnitSpriteByName("soldier")
                //    , () => {
                //        this.AddFactory();
                //        }
                //    )
                //);
                r = new RadialMenu( l );
                return true;
            }
            else return false;
        }
        void AddSoldier()
        {
            ConsumeWorker();
            mUnitHolder.AddUnit( mUnitTypeHolder.BuildUnitByTypeName("infantry", Origin, mOwner));
        }
        void AddFoundry()
        {
            Vector2 pos = ConsumeWorker();
            Foundry ev = new Foundry( pos, mOwner );
            if ( mMaps.TrySetMapEventAt( pos, ev ) )
            { 
                mMaps.SetTerrainAt( pos, eTerrainType.Foundry );
            }
            else
            { 
                FreeWorkers.Insert(0, pos);
                Debug.WriteLine("Cannot add an event where one exists!");
            }
        }
        Vector2 ConsumeWorker()
        {
            Vector2 pos = FreeWorkers[0];
            FreeWorkers.RemoveAt(0);
            return pos;
        }


        protected override string GetChildJson()
        {
            string result = ",\"Type\" : \"City\" ";
            result += ",\"Current\" : " + _Current;
            result += ",\"growthTickDown\" : " + growthTickDown;
            result += ",\"FreeWorkers\" : {";
            foreach ( Vector2 v in FreeWorkers )
            { 
                result += v.ToJson();
                result += ",";
            }
            result = result.Substring(0, result.Length - 1 );
            result += "}";
            return result;
        }
    }
}
