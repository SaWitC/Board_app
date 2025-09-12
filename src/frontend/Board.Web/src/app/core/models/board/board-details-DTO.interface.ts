export interface BoardDetailsDTO {
  id: string;
  title: string;
  description: string;
  boardUsers: { email: string; role: number; }[];
  boardColumns: { id?: string; title: string; description: string; }[];
  modificationDate: Date;
  IsTemplate: boolean;
  IsActiveTemplate: boolean;
}
