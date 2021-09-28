-- Galaxy Central Prison, Galactolus

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Other, o.Standing, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyPrisoners, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyPrisoners, q.Count, 2
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.Rest, 1
	elseif n == 2 then
		return a.OnCallFromPrison, p.HasPrompt, p.ForEnemy
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.Exists(2) and (obj.CanSB(1) or obj.CanCB(1)) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	local t = 0
	if n == 1 then
		obj.SoulCharge(3)
		obj.SetPrison()
	elseif n == 2 then
		if (obj.CanSB(1) and obj.CanCB(1)) then
			t = obj.SelectOption("Counter Blast 1", "Soul Blast 1")
		elseif (obj.CanSB(1)) then
			t = 2
		else
			t = 1
		end
		if t == 1 then
			obj.CounterBlast(1)
			obj.Free(3)
		elseif t == 2 then
			obj.SoulBlast(1)
			obj.Free(2)
		end
	end
	return 0
end