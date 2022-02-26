-- ディアブロスガールズ ステファニー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.InFinalRush() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.SameColumn}, cs.BonusPower, 5000, p.Continuous)
	end
end