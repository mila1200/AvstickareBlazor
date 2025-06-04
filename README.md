# 🌍 Avstickare – Frontend (Blazor WebAssembly PWA)
## Av: Mikael Larsson
### Mittuniversitetet, Självständigt arbete DT140G, vt 2025
Detta är frontend-applikationen till Avstickare – en Progressive Web App (PWA) byggd med Blazor WebAssembly. Den låter användaren planera bilresor, visa intressanta platser längs rutten, spara resor och favoritplatser samt interagera med en karta.

---

## 🛠 Teknikstack

- **Blazor WebAssembly
- **MudBlazor** – UI-komponentbibliotek
- **Progressive Web App (PWA)** – Web App Manifest
- **Google Maps JavaScript API** – Kartor, rutter och markörer
- **LocalStorage** – Tillfällig lagring av reseutkast och JWT
- **JWT-autentisering** – Inloggning och token-hantering

---

## 📦 Funktionalitet

- Ange start- och slutdestination
- Generera rutt och få platsförslag längs vägen
- Interaktiv karta med markörer och polyline
- Spara resor och favoriter (autentiserad användare)
- Installationsbar som PWA
- Responsivt och mobilvänligt gränssnitt

---

## 🌐 API-integration

Frontend kommunicerar med backend-API:t (ASP.NET Core API) för att hämta och spara data.  
Alla anrop sker via `HttpClient` och är uppdelade i services:

- `AuthService` 
- `TripService`
- `TripStopService`
- `FavoriteService`
- `PlaceService`
- `CustomAuthStateProvider`

Alla endpoints finns dokumenterade i backend-README (https://github.com/mila1200/AvstickareApi)

## 📱 Web App Manifest
```json
{
  "name": "Avstickare",
  "short_name": "Avstickare",
  "start_url": "/",
  "description": "Planera resor och hitta utflyktsmål med Avstickare",
  "scope": "/",
  "orientation": "portrait",
  "display": "standalone",
  "background_color": "#F8F9FA",
  "prefer_related_applications": false,
  "icons": [
    {
      "src": "icons/icon-192.png",
      "sizes": "192x192",
      "type": "image/png",
      "purpose": "any"
    },
    {
      "src": "icons/icon-512.png",
      "sizes": "512x512",
      "type": "image/png",
      "purpose": "any"
    },
    {
      "src": "icons/apple-touch-icon.png",
      "sizes": "180x180",
      "type": "image/png",
      "purpose": "any"
    },
    {
      "src": "icons/splash-1024.png",
      "sizes": "1024x1024",
      "type": "image/png",
      "purpose": "any"
    },
    {
      "src": "icons/favicon-32x32.png",
      "sizes": "32x32",
      "type": "image/png"
    },
    {
      "src": "icons/favicon-16x16.png",
      "sizes": "16x16",
      "type": "image/png"
    }
  ]
}
