-- ラーヴァワイヤ・ドラゴン

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.FrontRowRC)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardState({q.Location, l.PlayerUnits}, cs.CanAttackBackRowInSameColumn, p.Continuous)
end