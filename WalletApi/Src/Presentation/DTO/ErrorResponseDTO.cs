namespace WalletApi.Presentation.DTO;

public record ErrorResponseDto(
    int StatusCode,
    string Message,
    string? Trace = ""
);