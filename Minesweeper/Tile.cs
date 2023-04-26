﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
    internal class Tile : Button
    {
        private Texture2D aliveTex;
        private Texture2D deadTex;
        public int xpos { private set; get; }
        public int ypos { private set; get; }
        public bool alive { private set; get; }

        public bool clicked { private set; get; }

        private Texture2D[] numberTexture;
        private Texture2D[] mineTexture;

        public bool flagged { private set; get; }

        private int numberOfMinesNeighbour = 0;

        private enum AliveNext
        {
            alive,
            dead,
            tbd
        }

        private AliveNext aliveToChangeToNextTurn = AliveNext.tbd;

        public Tile(Rectangle _rectangle, Texture2D _texture, Texture2D _aliveTex, int _xpos, int _ypos, Texture2D[] _numberOfNeighbours, Texture2D[] _mineTexture)
            : base(_rectangle, _texture)
        {
            alive = false;
            aliveTex = _aliveTex;
            deadTex = _texture;
            xpos = _xpos;
            ypos = _ypos;
            numberTexture = _numberOfNeighbours;
            clicked = false;
            mineTexture = _mineTexture;
            flagged = false;
        }

        public void SetToNumberTex(int numberOfMines)
        {
            Texture = numberTexture[numberOfMines];
        }

        public bool Clicked(Vector2 mousePos, int numberOfMines)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (rectangle.Contains(mousePos))
                {
                    if (alive)
                    {
                        //Texture = aliveTex;
                        Texture = mineTexture[2];
                    }
                    else
                    {
                        SetToNumberTex(numberOfMines);
                        numberOfMinesNeighbour = numberOfMines;
                    }
                    clicked = true;
                    return true;
                }
            }
            return false;
        }

        public void ShowMines()
        {
            if (alive)
            {
                Texture = mineTexture[4];
            }
            else if (flagged)
            {
                Texture = mineTexture[3];
            }
        }

        public void SetClicked()
        {
            alive = Input.mouseClickingToAlive;
            if (alive)
            {
                Texture = aliveTex;
            }
            else
            {
                Texture = deadTex;
            }
        }

        public void SetClicked(int numberOfMines)
        {
            clicked = true;
            if (alive)
            {
                //Texture = aliveTex;
                Texture = mineTexture[2];
            }
            else
            {
                SetToNumberTex(numberOfMines);
            }
        }

        public bool RightClick()
        {
            if (rectangle.Contains(Input.myWorldMousePos))
            {
                if (!clicked)
                {
                    if (Texture == deadTex)
                    {
                        Texture = mineTexture[0];
                        flagged = true;
                    }
                    else
                    {
                        Texture = deadTex;
                        flagged = false;
                    }
                }
                return true;
            }
            return false;
        }

        public void SetClicked(bool _alive)
        {
            alive = _alive;
            if (alive)
            {
                Texture = aliveTex;
            }
            else
            {
                Texture = deadTex;
            }
        }

        public void SetAlive(bool _alive)
        {
            if (_alive)
            {
                aliveToChangeToNextTurn = AliveNext.alive;
            }
            else
            {
                aliveToChangeToNextTurn = AliveNext.dead;
            }
        }

        public void SetAlive(int aliveNeighboors)
        {
            if (aliveToChangeToNextTurn == AliveNext.tbd)
            {
                if (!alive && aliveNeighboors == 3)
                {
                    SetAlive(true);
                    return;
                }
                else if (alive && (aliveNeighboors < 2) ^ (aliveNeighboors > 3))
                {
                    SetAlive(false);
                    return;
                }
            }
        }

        public void UpdateAlive()
        {
            if (aliveToChangeToNextTurn != AliveNext.tbd)
            {
                if (aliveToChangeToNextTurn == AliveNext.alive)
                {
                    alive = true;
                }
                else if (aliveToChangeToNextTurn == AliveNext.dead)
                {
                    alive = false;
                }
                if (alive)
                {
                    Texture = aliveTex;
                }
                else
                {
                    Texture = deadTex;
                }
                aliveToChangeToNextTurn = AliveNext.tbd;
            }
        }
    }
}