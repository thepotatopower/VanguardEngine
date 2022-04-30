-- 大渓谷の探求家 C・K・ザカット

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.BackRowCenterRC)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Cont")
	-- on intercept
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PutOnGC)
	ability2.SetMovedFrom(l.BackRowCenterRC)
	ability2.SetTrigger("Trigger")
	ability2.SetCondition("Condition")
	ability2.SetActivation("Activation")
end

function Cont()
	obj.AddCardState({q.Other, o.This}, cs.CanInterceptFromBackRow, p.Continuous)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusShield, 10000, p.UntilEndOfBattle)
end