-- Looting Petal, Stomalia

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
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, false, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.NotActivatedYet() and obj.IsRearguard() and obj.CanCB(1) and obj.CanSB(2) then
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
		obj.AddSkill(3, 0)
		obj.AddTempPower(3, 5000)
	end
	return 0
end