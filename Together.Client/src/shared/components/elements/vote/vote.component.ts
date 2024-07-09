import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgClass } from '@angular/common';
import { BaseComponent } from '@/core/abstractions';
import { EVoteType } from '@/shared/enums';
import { ShortenNumberPipe } from '@/shared/pipes';

@Component({
  selector: 'together-vote',
  standalone: true,
  imports: [NgClass, ShortenNumberPipe],
  templateUrl: './vote.component.html',
})
export class VoteComponent extends BaseComponent {
  @Input()
  sourceId = '';

  @Input()
  voteFor: 'post' | 'reply' = 'post';

  @Input()
  voteUpCount = 0;

  @Input()
  voteDownCount = 0;

  @Input()
  replyCount = 0;

  @Input()
  voted?: EVoteType;

  @Output()
  expandReplyWriter = new EventEmitter<void>();
}
