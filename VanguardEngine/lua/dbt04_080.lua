-- スチームハンター ナヌール

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsPlayerTurn() and obj.SoulCount() >= 5 then
		obj.AddCardValue({q.Location, l.PlayerVC}, cs.BonusPower, 2000, p.Continuous)
	end
end