-- ビーティングハート カミラ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.FrontRowRC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.GetNumberOf("Filter", {l.SungThisTurn}) > 0 then
		obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 2000, p.Continuous)
	end
end

function Filter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_057")
end