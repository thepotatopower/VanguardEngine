-- Strong Fortress Dragon, Jibrabrachio

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Other, o.Standing, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.Count, 1, q.Other, o.OrLess
	elseif n == 5 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	elseif n == 2 then
		return a.OnBattlePhase, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.VanguardIs("Heavy Artillery of Dust Storm, Eugene") and obj.CanSB(1) and obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsPlayerTurn() and obj.Exists(4) then
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
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
		obj.Rest(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(3)
	elseif n == 2 then
		obj.Stand(5)
		obj.AddTempPower(5, 5000)
	end
	return 0
end