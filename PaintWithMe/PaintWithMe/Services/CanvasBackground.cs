using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PaintWithMe
{
    class CanvasBackground : IService
    {
        private Color _backgroundColor;

        private bool _activateRed = false;
        private bool _activateBlue = false;
        private bool _activateGreen = false;

        PaintWithMeGame _game;

        public void Initialize()
        {
            SetBackgroundColor();

            _game = ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game);
        }

        public ServiceType GetServiceType()
        {
            return ServiceType.CanvasBackground;
        }

        public Color GetBackgroundColor()
        {
            if (_game.DisplayBlackAndWhite)
            {
                return Color.LightGray;
            }
            else
            {
                return _backgroundColor;
            }
        }

        public void ActivateRed(bool bActivate)
        {
            _activateRed = bActivate;
            SetBackgroundColor();
        }

        public void ActivateBlue(bool bActivate)
        {
            _activateBlue = bActivate;
            SetBackgroundColor();
        }

        public void ActivateGreen(bool bActivate)
        {
            _activateGreen = bActivate;
            SetBackgroundColor();
        }

        protected void SetBackgroundColor()
        {
            //there are 8 combinations of the colors

            if (_activateRed)
            {
                if (_activateBlue)
                {
                    if (_activateGreen)
                    {
                        //RGB
                        _backgroundColor = Color.DarkGray;
                    }
                    else //if (!_activateGreen)
                    {
                        //R_B
                        _backgroundColor = Color.MediumPurple;
                    }
                }
                else //if (!_activateBlue)
                {
                    if (_activateGreen)
                    {
                        //RG_
                        _backgroundColor = Color.Khaki;
                    }
                    else //if (!_activateGreen)
                    {
                        //R__
                        _backgroundColor = Color.PaleVioletRed;
                    }
                }
            }
            else //if (!_activateRed)
            {
                if (_activateBlue)
                {
                    if (_activateGreen)
                    {
                        //_GB
                        _backgroundColor = Color.LightCyan;
                    }
                    else //if (!_activateGreen)
                    {
                        //__B
                        _backgroundColor = Color.LightSkyBlue;
                    }
                }
                else //if (!_activateBlue)
                {
                    if (_activateGreen)
                    {
                        //_G_
                        _backgroundColor = Color.LightGreen;
                    }
                    else //if (!_activateGreen)
                    {
                        //___
                        _backgroundColor = Color.White;
                    }
                }
            }
        }
    }
}
