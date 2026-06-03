import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserMessages } from '../constants/user-messages';
import { ResumeAnalyzerError } from '../errors/resume-analyzer.error';
import {
  AnalyzeResumeRequest,
  ResumeAnalysisResponse,
  ValidationProblemResponse
} from '../models';

@Injectable({ providedIn: 'root' })
export class ResumeAnalyzerService {
  /** Backend analyze endpoint — HTTP only, port 5136. */
  private static readonly ANALYZE_URL = 'http://localhost:5136/api/resume/analyze';

  private readonly http = inject(HttpClient);

  analyze(request: AnalyzeResumeRequest): Observable<ResumeAnalysisResponse> {
    return this.http
      .post<ResumeAnalysisResponse>(ResumeAnalyzerService.ANALYZE_URL, request)
      .pipe(
        map((response) => this.normalizeResponse(response)),
        catchError((error: unknown) => throwError(() => this.toUserError(error)))
      );
  }

  private normalizeResponse(response: ResumeAnalysisResponse): ResumeAnalysisResponse {
    return {
      summary: response.summary ?? '',
      detectedSkills: response.detectedSkills ?? [],
      missingSkills: response.missingSkills ?? [],
      feedback: response.feedback ?? [],
      linkedInHeadline: response.linkedInHeadline ?? '',
      improvementSuggestions: response.improvementSuggestions ?? []
    };
  }

  private toUserError(error: unknown): ResumeAnalyzerError {
    if (error instanceof ResumeAnalyzerError) {
      return error;
    }

    if (!(error instanceof HttpErrorResponse)) {
      return new ResumeAnalyzerError('server', UserMessages.serverError);
    }

    if (error.status === 0) {
      return new ResumeAnalyzerError('network', UserMessages.networkError);
    }

    if (error.status === 400) {
      return new ResumeAnalyzerError('validation', this.extractValidationMessage(error));
    }

    if (error.status >= 500) {
      return new ResumeAnalyzerError('server', UserMessages.serverError);
    }

    return new ResumeAnalyzerError('server', UserMessages.serverError);
  }

  private extractValidationMessage(error: HttpErrorResponse): string {
    const body = error.error as ValidationProblemResponse | undefined;

    if (!body?.errors) {
      return body?.detail ?? UserMessages.validationFallback;
    }

    const messages = Object.values(body.errors).flat().filter(Boolean);
    return messages[0] ?? UserMessages.validationFallback;
  }
}
