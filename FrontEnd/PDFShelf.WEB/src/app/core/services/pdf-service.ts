import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PdfSummary } from '../models/pdfs-model';

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  private http = inject(HttpClient);
  
  // Ajuste a porta se necessário (a sua estava em 5212)
  private apiUrl = 'http://localhost:5212/api/pdfs';

  // 1. Listar meus PDFs
  getMyPdfs(): Observable<PdfSummary[]> {
    return this.http.get<PdfSummary[]>(this.apiUrl);
  }

  // 2. Fazer Upload
  uploadPdf(file: File, title: string): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('title', title);

    // O Angular define o Content-Type 'multipart/form-data' automaticamente
    // quando passamos um FormData. Não precisamos definir headers manuais.
    return this.http.post(this.apiUrl, formData);
  }

  // 3. Deletar PDF (Vamos precisar em breve)
  deletePdf(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}