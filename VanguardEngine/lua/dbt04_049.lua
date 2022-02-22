-- 塒逆の騎士 ヌアザ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("OnAttackTrigger")
	ability1.SetCondition("OnAttackCondition")
	ability1.SetActivation("OnAttack")
	ability1.SetProperty(p.OncePerTurn)
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone() and not obj.Exists({q.Location, l.PlayerRC, q.Location, l.EnemyRC, q.Other, o.NotThisFieldID})
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	local timedTrigger = GiveAbility(GetID(), GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetActivation("TimedTrigger")
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
	timedTrigger.SetResetTarget(GetID())
end

function TimedTrigger()
	obj.Stand({q.Other, o.ThisFieldID})
end