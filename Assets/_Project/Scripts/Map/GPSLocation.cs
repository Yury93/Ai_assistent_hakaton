using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Yandex.Geocoder;
using Yandex.Geocoder.Enums;

public class GPSLocation : MonoBehaviour 
{



    private string geocoderURL = "http://geocoder.api.yandex.net";

    private IEnumerator Start()
    {
        yield return StartCoroutine(GeocodeAddress("Лукино 13"));
    }

    private IEnumerator GeocodeAddress(string address)
    {
       var adr = WWW.EscapeURL(address);
        string requestURL = $"https://geocode-maps.yandex.ru/1.x/?apikey=e111d317-32fe-4557-8470-31eaf437b32f&geocode=г+Балшиха,ул.Лукино,+дом+53а&format=json";

        UnityWebRequest www = UnityWebRequest.Get(requestURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            GeocoderResponse response = JsonUtility.FromJson<GeocoderResponse>(www.downloadHandler.text);

            if (response != null && response.Response.GeoObjectCollection.FeatureMember.Count > 0)
            {
                var firstGeoObject = response.Response.GeoObjectCollection.FeatureMember[0];
                var coordinate = firstGeoObject.GeoObject.Point.Pos;
                var addressComponents = firstGeoObject.GeoObject.MetaDataProperty.GeocoderMetaData.Address.Components;
                var country = addressComponents.FirstOrDefault(c => c.Kind.Equals(AddressComponentKind.Country));
                var province = addressComponents.LastOrDefault(c => c.Kind.Equals(AddressComponentKind.Province));
                var area = addressComponents.FirstOrDefault(c => c.Kind.Equals(AddressComponentKind.Area));
                var city = addressComponents.FirstOrDefault(c => c.Kind.Equals(AddressComponentKind.Locality));
                var street = addressComponents.FirstOrDefault(c => c.Kind.Equals(AddressComponentKind.Street));
                var house = addressComponents.FirstOrDefault(c => c.Kind.Equals(AddressComponentKind.House));

                Debug.Log($"Country: {country.Name}");
                Debug.Log($"Province: {province.Name}");
                Debug.Log($"Area: {area.Name}");
                Debug.Log($"City: {city.Name}");
                Debug.Log($"Street: {street.Name}");
                Debug.Log($"House: {house.Name}");
                Debug.Log($"Coordinate: {coordinate}");

                // Добавьте здесь свои дальнейшие действия с полученными данными
            }
            else
            {
                Debug.Log("Response doesn't contain valid data. " + www.downloadHandler.text);
            }
        }
        else
        {
            Debug.Log("Error retrieving geolocation data");
        }
    }
}

