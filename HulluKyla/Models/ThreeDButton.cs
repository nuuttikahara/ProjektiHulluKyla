using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models.Controls

    // Nappien animointi
{
    class ThreeDButton : Button
    {
        public ThreeDButton() {
            base.Pressed += OnPressedAnimation;
        }

        private async void OnPressedAnimation(object sender, EventArgs e) {
            // Animoidaan painallus
            await Task.WhenAll(
                this.TranslateTo(2, 2, 50),
                this.ScaleTo(0.97, 50)
            );

            await Task.Delay(80);

            await Task.WhenAll(
                this.TranslateTo(0, 0, 50),
                this.ScaleTo(1, 50)
            );
        }
    }
}
