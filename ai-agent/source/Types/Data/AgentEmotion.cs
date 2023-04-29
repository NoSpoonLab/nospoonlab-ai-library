using System;

namespace AIAgent.Types.Data
{
    /// <summary>
    /// This enum lists the different emotional states that an AI agent can experience.
    /// </summary>
    public enum AgentEmotion
    {
        Joy,
        Sadness,
        Anger,
        Fear,
        Surprise,
        Disgust,
        Love,
        Happiness,
        Worry,
        Gratitude,
        Calm,
        Neutral
    }
    
    public static class AgentEmotionExtensions
    {
        public static string GetVoiceStyle(this AgentEmotion emotion)
        {
            switch (emotion)
            {
                case AgentEmotion.Joy:
                    return "cheerful";
                case AgentEmotion.Sadness:
                    return "sad";
                case AgentEmotion.Anger:
                    return "angry";
                case AgentEmotion.Fear:
                    return "terrified";
                case AgentEmotion.Surprise:
                    return "excited";
                case AgentEmotion.Disgust:
                    return "unfriendly";
                case AgentEmotion.Love:
                    return "friendly";
                case AgentEmotion.Happiness:
                    return "excited";
                case AgentEmotion.Worry:
                    return "fear";
                case AgentEmotion.Gratitude:
                    return "cheerful";
                case AgentEmotion.Calm:
                    return "whispering";
                case AgentEmotion.Neutral:
                    return "chat";
                default:
                    return "chat";
            }
        }
        
        public static AgentEmotion ToAgentEmotion(this string emotion)
        {
            return (AgentEmotion) Enum.Parse(typeof(AgentEmotion), emotion, true);
        }
    }
}