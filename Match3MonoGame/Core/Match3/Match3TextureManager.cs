using Match3MonoGame.Core.Match3.CellGrid;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Match3MonoGame.Core.Match3
{

    public static class Match3TextureManager
    {
        readonly private static Dictionary<CellType, string> _pathsTile = new Dictionary<CellType, string>()
        {
            [CellType.Black]    = "Tiles black/tileBlack_25",
            [CellType.Blue]     = "Tiles blue/tileBlue_28",
            [CellType.Green]    = "Tiles green/tileGreen_29",
            [CellType.Grey]     = "Tiles grey/tileGrey_41",
            [CellType.Orange]   = "Tiles orange/tileOrange_32",
            [CellType.Pink]     = "Tiles pink/tilePink_36",
            [CellType.Red]      = "Tiles red/tileRed_42",
            [CellType.Yellow]   = "Tiles yellow/tileYellow_45"
        };

        readonly private static Dictionary<CellBackType, string> _pathsTileBack = new Dictionary<CellBackType, string>()
        {
            [CellBackType.Blue]     = "Back tiles/BackTile_08",
            [CellBackType.Green]    = "Back tiles/BackTile_12",
            [CellBackType.Yellow]   = "Back tiles/BackTile_02",
        };

        readonly private static Dictionary<CellBonus, string> _pathBonusTile = new Dictionary<CellBonus, string>()
        {
            [CellBonus.HorizontalLine]  = "HorizontalBonus",
            [CellBonus.VerticalLine] = "VerticalBonus",
            [CellBonus.Bomb] = "Bomb"
        };

        readonly private static List<string> _pathExplotion = new List<string>
        {
            "Simple explosion/simpleExplosion00",
            "Simple explosion/simpleExplosion01",
            "Simple explosion/simpleExplosion02",
            "Simple explosion/simpleExplosion03",
            "Simple explosion/simpleExplosion04",
            "Simple explosion/simpleExplosion05",
            "Simple explosion/simpleExplosion06",
            "Simple explosion/simpleExplosion07",
            "Simple explosion/simpleExplosion08"
        };

        readonly private static string _pathBackgroundTile = "Back tiles/BackTile_15";

        readonly private static string _pathDefaultFont = "Font/KenneyFont";

        readonly private static string _pathParticle = "Particles yellow/particleYellow_4";

        private readonly static Dictionary<CellType, Texture2D> _texturesTile = new Dictionary<CellType, Texture2D>();
        private readonly static Dictionary<CellBackType, Texture2D> _texturesTileBack = new Dictionary<CellBackType, Texture2D>();
        private readonly static Dictionary<CellBonus, Texture2D> _textureBonus = new Dictionary<CellBonus, Texture2D>();
        private readonly static List<Texture2D> _textureExpotion = new List<Texture2D>();
        private static Texture2D _textureBackgrounTile = null;
        private static SpriteFont _defaultFont = null;
        private static Texture2D _textureParticle = null;


        public static Texture2D GetTextureTile(CellType type) => _texturesTile[type];
        public static Texture2D GetTextureTileBack(CellBackType type) => _texturesTileBack[type];
        public static Texture2D GetTextureBackgroundTile() => _textureBackgrounTile;

        public static Texture2D GetTextureBonusTile(CellBonus bonus) => _textureBonus[bonus];
        public static SpriteFont GetDefaultFont() => _defaultFont;

        public static Texture2D GetTextureParticle() => _textureParticle;

        public static List<Texture2D> GetTexturesExplotion() => _textureExpotion;

        public static void LoadTextures(ContentManager content)
        {
            foreach (var tile in _pathsTile)
            {
                _texturesTile.Add(tile.Key, content.Load<Texture2D>(tile.Value));
            }

            foreach (var tile in _pathsTileBack)
            {
                _texturesTileBack.Add(tile.Key, content.Load<Texture2D>(tile.Value));
            }

            foreach (var tile in _pathBonusTile)
            {
                _textureBonus.Add(tile.Key, content.Load<Texture2D>(tile.Value));
            }

            foreach (var path in _pathExplotion)
            {
                _textureExpotion.Add(content.Load<Texture2D>(path));
            }
            _textureBackgrounTile = content.Load<Texture2D>(_pathBackgroundTile);
            _defaultFont = content.Load<SpriteFont>(_pathDefaultFont);
            _textureParticle = content.Load<Texture2D>(_pathParticle);
        }
    }
}
