using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BasicMonoGame;


//Cette classe nous permet d'instancier un objet Sprite de notre jeu (le joueur et l'objectif dans notre cas)
public class Sprite
{
    private Texture2D _texture;
    private int _size;
    private Vector2 _position;
    private static readonly int _sizeMin = 10;
    private static readonly int _sizeMax = 128;
    private Color _color = Color.White;

    public Texture2D _Texture { get => _texture; init => _texture = value; }
    public int _Size {get => _size; set => _size = value>=_sizeMin && value<=_sizeMax?value:_sizeMin;}
    public Rectangle _Rect { get => new Rectangle((int)_position.X, (int)_position.Y, _size, _size); }
    
    public Vector2 _Position { get => _position; set => _position = value; }

    public Sprite(Texture2D texture, Vector2 position, int size)
    {
        _Texture = texture;
        _Position = position;
        _Size = size;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _Rect, _color);
    }
}