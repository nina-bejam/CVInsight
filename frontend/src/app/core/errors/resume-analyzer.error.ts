export type ResumeAnalyzerErrorKind = 'validation' | 'server' | 'network';

export class ResumeAnalyzerError extends Error {
  readonly kind: ResumeAnalyzerErrorKind;
  readonly userMessage: string;

  constructor(kind: ResumeAnalyzerErrorKind, userMessage: string) {
    super(userMessage);
    this.name = 'ResumeAnalyzerError';
    this.kind = kind;
    this.userMessage = userMessage;
  }
}
