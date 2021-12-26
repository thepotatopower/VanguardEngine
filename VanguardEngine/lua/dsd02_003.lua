-- ディアブロス “悪童” スティーブ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("PlacedOnVCTrigger")
	ability1.SetActivation("PlacedOnVC")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function PlacedOnVCTrigger()
	return obj.IsApplicable()
end

function PlacedOnVC()
	obj.SuperiorCallToSpecificCircle({q.Location, l.Soul, q.Count, 1}, FL.PlayerBackCenter)
	obj.SoulCharge(1)
end

function Cont()
	if obj.InFinalRush() then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end
