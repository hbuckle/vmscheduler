import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'resourcename'
})
export class ResourcenamePipe implements PipeTransform {

  transform(value: string, args?: any): string {
    var split = value.split("/");
    return split.pop();
  }

}
