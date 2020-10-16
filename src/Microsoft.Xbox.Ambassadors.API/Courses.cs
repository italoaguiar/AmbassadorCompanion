using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public static class Courses
    {
        private const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/academy/courses";

        public async static Task<Course[]> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Course[]>(token, new Uri(REQUEST_URI));
        }
    }

    public partial class Course
    {
        [JsonPropertyName("courseId")]
        public long CourseId { get; set; }

        [JsonPropertyName("courseMission")]
        public CourseMission CourseMission { get; set; }

        [JsonPropertyName("hasCompleted")]
        public bool HasCompleted { get; set; }

        [JsonPropertyName("percentComplete")]
        public long PercentComplete { get; set; }

        [JsonPropertyName("quizResults")]
        public IList<QuizResult> QuizResults { get; set; }

        [JsonPropertyName("honorUri")]
        public object HonorUri { get; set; }
    }

    public partial class CourseMission
    {
        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("specializationId")]
        public long SpecializationId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("threshold")]
        public long Threshold { get; set; }

        [JsonPropertyName("aggregateTarget")]
        public long AggregateTarget { get; set; }

        [JsonPropertyName("parentId")]
        public object ParentId { get; set; }

        [JsonPropertyName("missionTypeDefinitionId")]
        public long MissionTypeDefinitionId { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("seasonId")]
        public long SeasonId { get; set; }

        [JsonPropertyName("effectiveDate")]
        public DateTimeOffset EffectiveDate { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("iconUri")]
        public Uri IconUri { get; set; }

        [JsonPropertyName("actorId")]
        public object ActorId { get; set; }

        [JsonPropertyName("isQuizMission")]
        public bool IsQuizMission { get; set; }

        [JsonPropertyName("quizzesAvailable")]
        public long QuizzesAvailable { get; set; }

        [JsonPropertyName("hasAccolade")]
        public bool HasAccolade { get; set; }

        [JsonPropertyName("honorImageUri")]
        public object HonorImageUri { get; set; }
    }

    public partial class QuizResult
    {
        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("quizId")]
        public Guid QuizId { get; set; }

        [JsonPropertyName("quizCompleted")]
        public bool QuizCompleted { get; set; }

    }
}
