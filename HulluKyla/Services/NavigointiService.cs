using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Services {
    public static class NavigointiService {
        public static async Task Navigoi(string reitti) {
            try {
                if (string.IsNullOrWhiteSpace(reitti)) {
                    Console.WriteLine("Navigointivirhe: reitti ei saa olla tyhjä.");
                    return;
                }

                // Navigoi suoraan juureen
                await Shell.Current.GoToAsync($"//{reitti}");
            } catch (Exception ex) {
                Console.WriteLine($"Navigointivirhe reitissä '{reitti}': {ex.Message}");
                await Shell.Current.DisplayAlert("Virhe", "Navigointi epäonnistui.", "OK");
            }


        }
    }
}
