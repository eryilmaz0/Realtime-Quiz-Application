import { Pipe, PipeTransform } from '@angular/core';
import { UserModel } from '../Models/UserModel';

@Pipe({
  name: 'secondUser'
})
export class SecondUserPipe implements PipeTransform {

  transform(value: string): string {
    return "";
  }

}
