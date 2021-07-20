-- Blaze Fist Monk, Damari

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.FrontRowEnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 1, q.Min, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBattleEnds, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.CanCB(1) and obj.CanSB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanRetire(3) and obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(3)
		if obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") then
			obj.SuperiorCall(4)
		end
	end
	return 0
end
