using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public class Sweepstakes : INotifyPropertyChanged
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/sweepstakes";



        public static async Task<Sweepstakes[]> GetAsync(AccessToken token, bool upcoming)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string r = upcoming ? "?status=upcoming" : "?status=current";
            return await HttpUtil.GetAsync<Sweepstakes[]>(token, new Uri(REQUEST_URI + r));
        }

        public static async Task<Sweepstakes[]> GetExpiredAsync(AccessToken token, long seasonId)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Sweepstakes[]>(token, new Uri($"{REQUEST_URI}/{seasonId}"));
        }

        public static async Task<string> PostAsync(AccessToken token, long numberOfTickets, long sweepstakeDefinitionId)
        {
            var d = new {
                numberOfTickets = numberOfTickets,
                sweepstakeDefinitionId = sweepstakeDefinitionId
            };
            return await HttpUtil.PostAsync<string>(token, new Uri($"{REQUEST_URI}/enter"), d);
        }

        [JsonPropertyName("sweepstakeDefinitionId")]
        public long SweepstakeDefinitionId { get; set; }

        [JsonPropertyName("numberOfWinners")]
        public long NumberOfWinners { get; set; }

        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonPropertyName("drawingDate")]
        public object DrawingDate { get; set; }

        [JsonPropertyName("imageUrl")]
        public Uri ImageUrl { get; set; }


        private long? ticketsEntered;

        [JsonPropertyName("ticketsEntered")]
        public long? TicketsEntered
        {
            get => ticketsEntered;
            set
            {
                ticketsEntered = value;
                OnPropertyChanged(nameof(TicketsEntered));
            }
        }



        [JsonPropertyName("daysLeft")]
        public long? DaysLeft { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("winners")]
        public IList<string> Winners { get; set; }

        [JsonPropertyName("dateRange")]
        public object DateRange { get; set; }

        [JsonPropertyName("rewardDefinition")]
        public RewardDefinition RewardDefinition { get; set; }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }

    public partial class RewardDefinition
    {
        [JsonPropertyName("asset")]
        public object Asset { get; set; }

        [JsonPropertyName("rewardDefinitionId")]
        public long RewardDefinitionId { get; set; }

        [JsonPropertyName("specializationId")]
        public object SpecializationId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("rewardType")]
        public long RewardType { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("totalClaimedRewards")]
        public long TotalClaimedRewards { get; set; }

        [JsonPropertyName("assetId")]
        public object AssetId { get; set; }

        [JsonPropertyName("specialization")]
        public object Specialization { get; set; }
    }

}
