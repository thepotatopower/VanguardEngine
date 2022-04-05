-- ディアブロスジェットバッカー レナード

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on attack hits
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttackHits)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("Trigger")
	ability2.SetActivation("OnAttackHits")
end

function Cont()
	if obj.InFinalRush() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
		obj.AddCardState({q.Other, o.This}, cs.CanColumnAttack, p.Continuous)
	end
end

function Trigger()
	return obj.IsAttackingUnit()
end

function OnAttackHits()
	obj.SoulCharge(1)
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd02_001")}) then
		obj.SuperiorCallToSpecificCircle({q.Location, l.Soul, q.Count, 1, q.Min, 0}, FL.OpenCircle)
	end
end