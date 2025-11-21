export interface PdfSummary {
  id: string;
  title: string;
  thumbnailUrl: string;
  pageCount: number;
  fileSizeMB: number;
  uploadedAt: string; // Datas vêm como string do JSON
}
