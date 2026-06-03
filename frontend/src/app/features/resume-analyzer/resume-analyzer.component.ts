import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';
import { UserMessages } from '../../core/constants/user-messages';
import { ResumeAnalyzerError } from '../../core/errors/resume-analyzer.error';
import { ResumeAnalysisResponse } from '../../core/models';
import { ResumeAnalyzerService } from '../../core/services/resume-analyzer.service';
import { SAMPLE_RESUME, TARGET_ROLE_PRESETS } from '../../mock-data/sample-resume';

@Component({
  selector: 'app-resume-analyzer',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './resume-analyzer.component.html',
  styleUrl: './resume-analyzer.component.scss'
})
export class ResumeAnalyzerComponent {
  private readonly fb = inject(FormBuilder);
  private readonly analyzerService = inject(ResumeAnalyzerService);
  private readonly snackBar = inject(MatSnackBar);

  protected readonly loading = signal(false);
  protected readonly result = signal<ResumeAnalysisResponse | null>(null);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly hasSubmitted = signal(false);
  protected readonly rolePresets = TARGET_ROLE_PRESETS;
  protected readonly messages = UserMessages;

  protected readonly form = this.fb.nonNullable.group({
    resumeText: ['', [Validators.required, Validators.minLength(50), Validators.maxLength(20000)]],
    targetRole: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(120)]]
  });

  protected analyze(): void {
    this.hasSubmitted.set(true);
    this.errorMessage.set(null);

    const clientError = this.getClientValidationMessage();
    if (clientError) {
      this.errorMessage.set(clientError);
      this.form.markAllAsTouched();
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.errorMessage.set(UserMessages.validationFallback);
      return;
    }

    const { resumeText, targetRole } = this.form.getRawValue();

    this.loading.set(true);
    this.result.set(null);

    this.analyzerService
      .analyze({ resumeText, targetRole })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (response) => {
          this.result.set(response);
          this.errorMessage.set(null);
        },
        error: (err: unknown) => {
          const message =
            err instanceof ResumeAnalyzerError
              ? err.userMessage
              : UserMessages.serverError;

          this.errorMessage.set(message);
          this.result.set(null);
          this.snackBar.open(message, 'Dismiss', { duration: 6000 });
        }
      });
  }

  protected loadSample(): void {
    this.errorMessage.set(null);
    this.form.patchValue({
      resumeText: SAMPLE_RESUME,
      targetRole: 'Full Stack Developer'
    });
  }

  protected applyRolePreset(role: string): void {
    this.form.patchValue({ targetRole: role });
  }

  protected showError(controlName: 'resumeText' | 'targetRole'): boolean {
    const control = this.form.controls[controlName];
    return control.invalid && (control.touched || this.hasSubmitted());
  }

  protected getResumeTextError(): string | null {
    const control = this.form.controls.resumeText;
    if (!this.showError('resumeText')) return null;
    if (control.hasError('required')) return UserMessages.resumeRequired;
    if (control.hasError('minlength')) return UserMessages.resumeTooShort;
    return null;
  }

  protected getTargetRoleError(): string | null {
    const control = this.form.controls.targetRole;
    if (!this.showError('targetRole')) return null;
    if (control.hasError('required')) return UserMessages.targetRoleRequired;
    return null;
  }

  private getClientValidationMessage(): string | null {
    const { resumeText, targetRole } = this.form.getRawValue();

    if (!resumeText.trim()) {
      return UserMessages.resumeRequired;
    }

    if (!targetRole.trim()) {
      return UserMessages.targetRoleRequired;
    }

    return null;
  }
}
