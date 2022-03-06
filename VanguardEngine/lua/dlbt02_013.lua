-- 包み込む恩情 トルクエル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.WhiteWings)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfBattle)
	local timedTrigger = GiveAbility(GetID(), GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
	timedTrigger.SetResetTarget(GetID())
	timedTrigger.SetCondition("TimedTriggerCondition")
	timedTrigger.SetActivation("TimedTrigger")
end

function TimedTriggerCondition()
	return obj.IsSameZone()
end

function TimedTrigger()
	if obj.YesNo(obj.GetDescription(2)) then
		obj.AddToSoul({q.Other, o.ThisFieldID})
	end
end