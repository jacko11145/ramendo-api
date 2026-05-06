namespace Ramendo.Application.Submissions.DTOs;

public sealed record ShopSubmissionDto(
    string Id, string ShopName, string City, string District,
    string Address, string? Note, string Status,
    string SubmittedByUserId, string? SubmittedByName,
    DateTime SubmittedAt, DateTime? ReviewedAt, string? RejectionReason);

public sealed record CreateSubmissionDto(
    string ShopName, string City, string District,
    string Address, string? Note);

public sealed record RejectSubmissionRequest(string? Reason);
