using System.Linq.Expressions;
using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Features.FeatureConversation;

public static class ConversationExpressions
{
    public static Expression<Func<Conversation, ConversationViewModel>> ConversationViewModelSelector(CurrentUserClaims currentUserClaims)
    {
        return c => new ConversationViewModel
        {
            Id = c.Id,
            SubId = c.SubId,
            Type = c.Type,
            Name = c.Type == ConversationType.Group
                ? c.Name
                : c.ConversationParticipants
                    .FirstOrDefault(cp => cp.UserId != currentUserClaims.Id)!
                    .User.UserName,
            Image = c.Type == ConversationType.Group
                ? null
                : c.ConversationParticipants
                    .FirstOrDefault(cp => cp.UserId != currentUserClaims.Id)!
                    .User.Avatar,
            LastMessageByUserId = c.Messages!
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault()!
                .CreatedBy.Id,
            LastMessageByUserName = c.Messages!
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault()!
                .CreatedBy.UserName,
            LastMessageText = c.Messages!
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault()!
                .Text,
            LastMessageAt = c.Messages!
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault()!
                .CreatedAt
        };
    }
}