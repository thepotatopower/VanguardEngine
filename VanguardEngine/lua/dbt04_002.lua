-- 天輪真竜 マハーニルヴァーナ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCost("ACTCost")
	ability2.SetCanFullyResolve("ACTCanFullyResolve")
	ability2.SetActivation("ACT")
end

function Cont()
	if obj.IsPlayerTurn() and obj.Exists({q.Location, l.PlayerUnits, q.Other, o.OverDress}) then
		obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 10000, p.Continuous)
	end
end

function ACTCost(check)
	if check then return obj.CanCB(1) and obj.CanAddToDrop({q.Location, l.Hand, q.Location, l.Soul, q.NameContains, obj.LoadName("Nirvana")}) end
	obj.CounterBlast(1)
	obj.ChooseAddToDrop({q.Location, l.Hand, q.Location, l.Soul, q.NameContains, obj.LoadName("Nirvana"), q.Count, 1})
end

function ACTCanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Drop, q.Grade, 0, q.Count, 1}) or (obj.Exists({q.Location, l.EnemyVC, q.Other, o.CanChoose}) and obj.GetEnemyDamage() <= 4) 
end

function ACT()
	obj.SuperiorCall({q.Location, l.Drop, q.Grade, 0, q.Count, 1})
	obj.Select({q.Location, l.EnemyVC, q.Count, 1})
	if obj.Exists({q.Location, l.Selected}) and obj.GetEnemyDamage() <= 4 then
		obj.DealDamage(1)
	end
end