-- 必中の天矢 スパラーレ

function RegisterAbilities()
	-- on stand
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnStand)
	ability1.SetLocation(l.FrontRowRC)
	ability1.SetTrigger("OnStandTrigger")
	ability1.SetCost("OnStandCost")
	ability1.SetActivation("OnStand")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.BackRowRC)
	ability2.SetActivation("Cont")
end

function OnStandTrigger()
	return obj.IsApplicable() and obj.SourceIsVanguardAbility()
end

function OnStandCost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function OnStand()
	obj.Draw(1)
end

function Cont()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.SameColumn, q.Other, o.NotThis}, cs.BonusPower, 5000, p.Continuous)
	end
end