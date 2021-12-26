-- 樹角獣 リンブル

function RegisterAbilities()
	-- continuous
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.BackRowRC)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardValue({q.Location, l.BackRowRC}, cs.BonusPower, 2000, p.Continuous)
end