namespace AICore.Services.Types.Data
{
    /// <summary>
    /// Defines the roles associated with communication between the user and the GPT models.
    /// Each role reflects a type of interaction or source of message within the system.
    /// </summary>
    public enum AIRole
    {
        /// <summary>
        /// Represents the owner of the message made by the system (Normally used to give initial instructions).
        /// </summary>
        System,

        /// <summary>
        /// Represents the owner of the message made by the user.
        /// </summary>
        User,

        /// <summary>
        /// Represents the owner of the message performed by the generative pre-trained transformers model.
        /// </summary>
        Assistant,

        /// <summary>
        /// Represents the owner of the message associated with a system function.
        /// </summary>
        Tool
    }
}