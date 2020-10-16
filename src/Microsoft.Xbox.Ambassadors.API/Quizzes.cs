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
    public static class Quizzes
    {
        private const string BASE_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/academy/quizzes/";
        private const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/academy/quizzes?nonCourseQuizzesOnly=";
        private const string GRADE_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/academy/quizzes/grade/";

        public static async Task<Quizz[]> GetAsync(AccessToken token, bool nonCourseQuizzesOnly = true)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var c = nonCourseQuizzesOnly ? "true" : "false";
            return await HttpUtil.GetAsync<Quizz[]>(token, new Uri(REQUEST_URI + c));
        }

        public static async Task<Quizz> GetAsync(AccessToken token, string id)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Quizz>(token, new Uri(BASE_URI + id));
        }

        public static async Task<Quizz> PostAsync(AccessToken token, Quizz e)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (e == null)
                throw new ArgumentNullException(nameof(e));

            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("Referer", "https://ambassadors.microsoft.com/xbox/academy/" + e.Id);
            return await HttpUtil.PostAsync<Quizz>(token, new Uri(GRADE_URI), e, d);
        }
    }

    public partial class Quizz:INotifyPropertyChanged
    {
        private long? _score;

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; }

        [JsonPropertyName("passingScore")]
        public long PassingScore { get; set; }

        [JsonPropertyName("previouslyPassed")]
        public bool? PreviouslyPassed { get; set; }

        [JsonPropertyName("dateAdded")]
        public DateTimeOffset DateAdded { get; set; }

        [JsonPropertyName("dateUpdated")]
        public DateTimeOffset DateUpdated { get; set; }

        [JsonPropertyName("datePublished")]
        public DateTimeOffset DatePublished { get; set; }

        [JsonPropertyName("lastSavedBy")]
        public string LastSavedBy { get; set; }

        [JsonPropertyName("resources")]
        public IList<Resource> Resources { get; set; }

        [JsonPropertyName("questions")]
        public IList<Question> Questions { get; set; }

        [JsonPropertyName("allowDelete")]
        public bool AllowDelete { get; set; }

        [JsonPropertyName("score")]
        public long? Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public partial class Question
    {
        [JsonPropertyName("questionId")]
        public long QuestionId { get; set; }

        [JsonPropertyName("type")]
        public TypeEnum Type { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; }

        [JsonPropertyName("imageUrl")]
        public object ImageUrl { get; set; }

        [JsonPropertyName("isCorrect")]
        public bool? IsCorrect { get; set; }

        [JsonPropertyName("answers")]
        public IList<Answer> Answers { get; set; }
    }

    public partial class Answer
    {
        [JsonPropertyName("answerId")]
        public long AnswerId { get; set; }

        [JsonPropertyName("answerText")]
        public string AnswerText { get; set; }

        [JsonPropertyName("isCorrect")]
        public bool? IsCorrect { get; set; }

        [JsonPropertyName("isAnchored")]
        public bool? IsAnchored { get; set; }

        [JsonPropertyName("isSelected")]
        public bool? IsSelected { get; set; }
    }

    public partial class Resource
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("description")]
        public object Description { get; set; }
    }

    public enum TypeEnum { Multi, Single };

}
