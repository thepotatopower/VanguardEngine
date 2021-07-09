-- Galaxy Central Prison, Galactolus

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Other, o.Standing, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 4 then
		return q.Location, l.EnemyPrisoners, q.Count, 1
	elseif n == 5 then
		return q.Location, l.EnemyPrisoners, q.Count, 2
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnCallFromPrison, t.Order, p.HasPrompt, true, p.IsMandatory, true, p.ForEnemy
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.Exists(4) and (obj.CanSB(2) or obj.CanCB(3)) then
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

function Cost(n)
	if n == 1 then
		obj.ChooseRest(1)
	end
end

function Activate(n)
	local t = 0
	if n == 1 then
		obj.SoulCharge(3)
		obj.SetPrison()
	elseif n == 2 then
		if (obj.CanSB(2) and obj.CanCB(3)) then
			t = obj.SelectOption("Counter Blast 1", "Soul Blast 1")
		elseif (obj.CanSB(2)) then
			t = 2
		else
			t = 1
		end
		if t == 1 then
			obj.CounterBlast(3)
			obj.Free(5)
		elseif t == 2 then
			obj.SoulBlast(2)
			obj.Free(4)
		end
	end
	return 0
end