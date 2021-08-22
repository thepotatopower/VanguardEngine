-- Chakrabarthi Divine Dragon, Nirvana

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.UnitType, u.overDress
	elseif n == 3 then
		return q.Location, l.PlayerVC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, p.OncePerTurn, p.Discard, 1
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() then 
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1)
	elseif n == 2 then
		obj.AddTempPower(2, 10000)
		obj.AddTempPower(3, 10000)
	end
	return 0
end