-- 黒暗の騎士 オブスクデイト

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("OnAttackTrigger")
	ability1.SetCondition("OnAttackCondition")
	ability1.SetActivation("OnAttack")
	-- on place
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Glitter)
	ability2.SetTiming(a.PlacedOnRC)
	ability2.SetTrigger("OnPlaceTrigger")
	ability2.SetCondition("OnPlaceCondition")
	ability2.SetActivation("OnPlace")
	-- cont
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetProperty(p.Glitter)
	ability3.SetTiming(a.Cont)
	ability3.SetLocation(l.RC, l.GC)
	ability3.SetActivation("Cont")
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.GetNumberOf({q.Location, l.Order, q.Other, o.SetOrder}) >= 1 and obj.IsSameZone()
end

function OnAttack()
	local count = obj.GetNumberOf({q.Location, l.Order, q.Other, o.SetOrder})
	local power = 0
	if count >= 3 then
		power = 10000
	elseif count >= 1 then
		power = 5000
	end
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end

function OnPlaceTrigger()
	return obj.SourceIsAbility() and obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.GetNumberOf("GlitterFilter", {l.PlayerVC}) > 0 and obj.IsSameZone() and obj.GetNumberOf("Filter", {l.Order}) >= 3
end

function Filter(id) 
	return obj.IsOrderType(id, ot.Research)
end

function OnPlace()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusCritical, 1, p.UntilEndOfTurn)
end

function Cont()
	if obj.GetNumberOf("GlitterFilter", {l.PlayerVC}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Intercept, p.Continuous)
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 10000, p.Continuous)
	end
end

function GlitterFilter(id) 
	return obj.NameContains(id, obj.LoadName("Eva")) and obj.HasProperty(id, p.IsGlitter)
end