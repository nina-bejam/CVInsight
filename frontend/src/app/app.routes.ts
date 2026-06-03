import { Routes } from '@angular/router';
import { ResumeAnalyzerComponent } from './features/resume-analyzer/resume-analyzer.component';

export const routes: Routes = [
  { path: '', component: ResumeAnalyzerComponent },
  { path: '**', redirectTo: '' }
];
