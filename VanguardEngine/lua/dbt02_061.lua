-- Stealth Dragon, Kizanreiji

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Other, o.SameColumn, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and not obj.Activated() and obj.CanCB(1) and obj.CanSB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanRetire(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(3)
	end
	return 0
end