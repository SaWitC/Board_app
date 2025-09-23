export interface BoardDetailsDTO {
  id: string;
  title: string;
  description: string;
  boardUsers: { email: string; role: number; }[];
  modificationDate: Date;
  isTemplate: boolean;
  IsActiveTemplate: boolean;
}
