namespace Ramendo.Application.Submissions.DTOs;

public sealed record ShopSubmissionDto(
    string Id, string Name, string? Description, string City, string District,
    string DetailAddress, string? Phone, string? Website, string[] Images, string[] Types,
    string Status, string? Feedback, string UserId, DateTime CreatedAt);

public sealed record CreateSubmissionDto(
    string Name, string? Description, string City, string District,
    string DetailAddress, string? Phone, string? Website, string? FacebookPageId,
    string[] Images, string[] Types);
