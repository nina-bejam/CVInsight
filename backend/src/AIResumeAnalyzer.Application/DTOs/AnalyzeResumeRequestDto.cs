using System.ComponentModel.DataAnnotations;

namespace AIResumeAnalyzer.Application.DTOs;

public sealed class AnalyzeResumeRequestDto
{
    [Required(ErrorMessage = "Resume text is required.")]
    [MinLength(50, ErrorMessage = "Resume text must be at least 50 characters.")]
    [MaxLength(20000, ErrorMessage = "Resume text cannot exceed 20,000 characters.")]
    public string ResumeText { get; init; } = string.Empty;

    [Required(ErrorMessage = "Target role is required.")]
    [MinLength(2, ErrorMessage = "Target role must be at least 2 characters.")]
    [MaxLength(120, ErrorMessage = "Target role cannot exceed 120 characters.")]
    public string TargetRole { get; init; } = string.Empty;
}
