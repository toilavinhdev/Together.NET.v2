import { booleanAttribute, Component, Input } from '@angular/core';
import { IPostViewModel } from '@/shared/entities/post.entities';
import { NgClass } from '@angular/common';
import { BaseComponent } from '@/core/abstractions';
import { AvatarComponent, PrefixComponent } from '@/shared/components/elements';
import { ShortenNumberPipe, TimeAgoPipe } from '@/shared/pipes';

@Component({
  selector: 'together-post',
  standalone: true,
  imports: [
    NgClass,
    AvatarComponent,
    PrefixComponent,
    TimeAgoPipe,
    ShortenNumberPipe,
  ],
  templateUrl: './post.component.html',
})
export class PostComponent extends BaseComponent {
  @Input()
  post!: IPostViewModel;

  @Input({ transform: booleanAttribute })
  showTopicName = false;

  @Input({ transform: booleanAttribute })
  showReplyCount = true;
}
