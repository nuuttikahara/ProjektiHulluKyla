using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Services {
    public static class NavigointiService {
        public static async Task Navigoi(string reitti) {
            if (string.IsNullOrWhiteSpace(reitti)) {
                Console.WriteLine("Navigointivirhe: reitti ei saa olla tyhjä.");
                return;
            }

            try {
                // Luodaan while looppi, joka odottaa max 10 kertaa, että laite ehtii luoda navigointireitin.
                int yritys = 0;
                while ((Shell.Current?.Navigation == null) && yritys < 10) {
                    await Task.Delay(50);
                    yritys++;
                }

                if (Shell.Current?.Navigation == null) {
                    Console.WriteLine("Navigointi epäonnistui: Shell ei alustunut ajoissa.");
                    return;
                }

                await Shell.Current.GoToAsync($"//{reitti}");
            } catch (Exception ex) {
                Console.WriteLine($"Navigointivirhe reitissä '{reitti}': {ex.Message}");
                await Shell.Current.DisplayAlert("Virhe", "Navigointi epäonnistui.", "OK");
            }
        }
    }
}
