import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'QustioneType'
})
export class QustioneTypePipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    switch (value) {
      case 1:
        value = '單選';
        break;
      case 2:
        value = '多選';
        break;
      case 3:
        value = '自由填文字';
        break;
      case 4:
        value = '數字';
        break;
      default:
    }

    return value;
  }

}

@Pipe({
  name: 'Status'
})
export class StatusPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    switch (value) {
      case 0:
        value = '停用';
        break;
      case 1:
        value = '啟用';
        break;
      default:
    }

    return value;
  }

}
